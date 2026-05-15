import { Button } from "@/components/ui/button";
import { FieldGroup, Field, FieldLabel, FieldError } from "@/components/ui/field";
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
import { createFactionSchema } from "../faction.schemas";
import { useCreateFaction } from "../faction.queries";
import { Spinner } from "@/components/ui/spinner";
import { ApiError } from "@/lib/api-client";

export function CreateFaction({ onSuccess }: { onSuccess?: () => void }) {
  const createFaction = useCreateFaction();
  const form = useForm({
    defaultValues: {
      name: "",
      grandAlliance: "",
    },
    validators: {
      onChange: createFactionSchema,
      onSubmitAsync: async ({ value }) => {
        try {
          const parsed = createFactionSchema.parse(value);
          await createFaction.mutateAsync(parsed);
          return null;
        } catch (err) {
          if (err instanceof ApiError && err.status === 409) {
            return {
              fields: {
                name: { message: "Name already used by another faction" },
              },
            };
          }
          if (err instanceof ApiError && err.status === 403) {
            return { form: { message: "You don't have permission to do this." } };
          }
          return { form: { message: "Unexpected error" } };
        }
      },
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
                {isSubmitting ? <Spinner /> : "Create"}
              </Button>
            </Field>
          )}
        />
      </FieldGroup>
    </form>
  );
}
