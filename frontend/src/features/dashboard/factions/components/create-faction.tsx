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

export function CreateFaction() {
  const form = useForm({
    defaultValues: {
      name: "",
      grandAlliance: "",
    },
    validators: { onChange: createFactionSchema },
    onSubmit: async ({ value }) => {
      await Promise.resolve();
      console.log(value);
      alert("submit success");
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
            return (
              <Field className="max-w-48">
                <FieldLabel>Grand Alliance</FieldLabel>
                <Select
                  items={grandAlliances}
                  onValueChange={(value) => {
                    field.handleChange(value ?? "");
                  }}
                  value={field.state.value}
                >
                  <SelectTrigger>
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
              </Field>
            );
          }}
        />
        <Field>
          <Button type="submit">Create</Button>
        </Field>
      </FieldGroup>
    </form>
  );
}
