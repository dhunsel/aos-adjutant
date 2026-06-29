import { FieldGroup, Field, FieldLabel, FieldError } from "@/components/ui/field";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import {
  Select,
  SelectValue,
  SelectTrigger,
  SelectGroup,
  SelectContent,
  SelectItem,
} from "@/components/ui/select";
import { useForm } from "@tanstack/react-form";
import { Spinner } from "@/components/ui/spinner";
import { createFactionSchema } from "../faction.schemas";

export interface FactionFormErrors {
  form?: { message: string };
  fields?: Partial<{ name: { message: string } }>;
}

export interface FactionFormFields {
  name: string;
  grandAlliance: string;
}

export function FactionForm({
  defaultValues,
  submitLabel,
  onSubmitAsync,
  onSuccess,
}: {
  defaultValues: FactionFormFields;
  submitLabel: string;
  onSubmitAsync: ({ value }: { value: FactionFormFields }) => Promise<FactionFormErrors | null>;
  onSuccess?: () => void;
}) {
  const form = useForm({
    defaultValues: {
      name: defaultValues.name,
      grandAlliance: defaultValues.grandAlliance,
    },
    validators: {
      onChange: createFactionSchema,
      onSubmitAsync: onSubmitAsync,
    },
    onSubmit: ({ formApi }) => {
      formApi.reset();
      onSuccess?.();
    },
  });

  const grandAlliances = [
    { label: "Order", value: "order" },
    { label: "Chaos", value: "chaos" },
    { label: "Death", value: "death" },
    { label: "Destruction", value: "destruction" },
  ];

  return (
    <form
      onSubmit={(e) => {
        e.preventDefault();
        void form.handleSubmit();
      }}
    >
      <FieldGroup>
        <form.Field
          name="name"
          children={(field) => {
            const isInvalid = field.state.meta.isTouched && !field.state.meta.isValid;
            return (
              <Field data-invalid={isInvalid}>
                <FieldLabel htmlFor={field.name}>Name</FieldLabel>
                <Input
                  id={field.name}
                  name={field.name}
                  value={field.state.value}
                  onBlur={field.handleBlur}
                  onChange={(e) => {
                    field.handleChange(e.target.value);
                  }}
                  aria-invalid={isInvalid}
                  autoComplete="off"
                />
                {isInvalid && <FieldError errors={field.state.meta.errors} />}
              </Field>
            );
          }}
        />
        <form.Field
          name="grandAlliance"
          children={(field) => {
            const isInvalid = field.state.meta.isTouched && !field.state.meta.isValid;
            return (
              <Field data-invalid={isInvalid} className="max-w-40">
                <FieldLabel htmlFor={field.name}>Grand Alliance</FieldLabel>
                <Select
                  name={field.name}
                  items={grandAlliances}
                  onOpenChange={(isOpen) => {
                    if (!isOpen) field.handleBlur();
                  }}
                  onValueChange={(value) => {
                    field.handleChange(value ?? "");
                  }}
                  value={field.state.value}
                >
                  <SelectTrigger id={field.name} aria-invalid={isInvalid}>
                    <SelectValue placeholder="Grand Alliance" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectGroup>
                      {grandAlliances.map((ga) => (
                        <SelectItem key={ga.value} value={ga.value}>
                          {ga.label}
                        </SelectItem>
                      ))}
                    </SelectGroup>
                  </SelectContent>
                </Select>
                {isInvalid && <FieldError errors={field.state.meta.errors} />}
              </Field>
            );
          }}
        />
        <form.Subscribe
          selector={(state) => [state.canSubmit && !state.isPristine, state.isSubmitting]}
          children={([canSubmitAndNotPristine, isSubmitting]) => (
            <Field>
              <Button disabled={!canSubmitAndNotPristine || isSubmitting} type="submit">
                {isSubmitting ? <Spinner /> : submitLabel}
              </Button>
            </Field>
          )}
        />
      </FieldGroup>
    </form>
  );
}
