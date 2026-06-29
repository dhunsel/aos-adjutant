import { useForm } from "@tanstack/react-form";
import { createAbilitySchema } from "../../ability.schemas";
//import type { Phase, Restriction, Turn } from "@/types/api.types";
import { Field, FieldError, FieldGroup, FieldLabel } from "@/components/ui/field";
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Button } from "@/components/ui/button";
import { Spinner } from "@/components/ui/spinner";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";

export interface AbilityFormErrors {
  form?: { message: string };
  fields?: Partial<{ name: { message: string } }>;
}

export interface AbilityFormFields {
  name: string;
  reaction: string;
  declaration: string;
  effect: string;
  phase: string;
  restriction: string;
  turn: string;
}

export function AbilityForm({
  defaultValues,
  submitLabel,
  onSubmitAsync,
  onSuccess,
}: {
  defaultValues: AbilityFormFields;
  submitLabel: string;
  onSubmitAsync: ({ value }: { value: AbilityFormFields }) => Promise<AbilityFormErrors | null>;
  onSuccess?: () => void;
}) {
  const form = useForm({
    defaultValues: {
      name: defaultValues.name,
      reaction: defaultValues.reaction,
      declaration: defaultValues.declaration,
      effect: defaultValues.effect,
      phase: defaultValues.phase,
      restriction: defaultValues.restriction,
      turn: defaultValues.turn,
    },
    validators: {
      onChange: createAbilitySchema,
      onSubmitAsync: onSubmitAsync,
    },
    onSubmit: ({ formApi }) => {
      formApi.reset();
      onSuccess?.();
    },
  });

  const phases = [
    { label: "Deployment", value: "deployment" },
    { label: "Start of Battle Round", value: "start" },
    { label: "Hero", value: "hero" },
    { label: "Movement", value: "movement" },
    { label: "Shooting", value: "shooting" },
    { label: "Charge", value: "charge" },
    { label: "Combat", value: "combat" },
    { label: "End of Turn", value: "end" },
    { label: "Passive", value: "passive" },
  ];

  const restrictions = [
    { label: "Once per turn (Army)", value: "onceTurnArmy" },
    { label: "Once per battle round (Army)", value: "onceRoundArmy" },
    { label: "Once per battle (Army)", value: "onceBattleArmy" },
    { label: "Once per battle round", value: "onceRound" },
    { label: "Once per battle", value: "onceBattle" },
  ];

  const turns = [
    { label: "Your Turn", value: "yourTurn" },
    { label: "Enemy Turn", value: "enemyTurn" },
    { label: "Any Turn", value: "anyTurn" },
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
        <div className="flex flex-col gap-3 md:flex-row">
          <form.Field
            name="turn"
            children={(field) => {
              const isInvalid = field.state.meta.isTouched && !field.state.meta.isValid;
              return (
                <Field data-invalid={isInvalid} className="flex-2">
                  <FieldLabel htmlFor={field.name}>Turn</FieldLabel>
                  <Select
                    name={field.name}
                    items={turns}
                    onOpenChange={(isOpen) => {
                      if (!isOpen) field.handleBlur();
                    }}
                    onValueChange={(value) => {
                      field.handleChange(value ?? "");
                    }}
                    value={field.state.value}
                  >
                    <SelectTrigger id={field.name} aria-invalid={isInvalid}>
                      <SelectValue placeholder="Turn" />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectGroup>
                        {turns.map((ga) => (
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
          <form.Field
            name="phase"
            children={(field) => {
              const isInvalid = field.state.meta.isTouched && !field.state.meta.isValid;
              return (
                <Field data-invalid={isInvalid} className="flex-3">
                  <FieldLabel htmlFor={field.name}>Phase</FieldLabel>
                  <Select
                    modal={true}
                    name={field.name}
                    items={phases}
                    onOpenChange={(isOpen) => {
                      if (!isOpen) field.handleBlur();
                    }}
                    onValueChange={(value) => {
                      field.handleChange(value ?? "");
                    }}
                    value={field.state.value}
                  >
                    <SelectTrigger id={field.name} aria-invalid={isInvalid}>
                      <SelectValue placeholder="Phase" />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectGroup>
                        {phases.map((ga) => (
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
        </div>
        <form.Field
          name="restriction"
          children={(field) => {
            const isInvalid = field.state.meta.isTouched && !field.state.meta.isValid;
            return (
              <Field data-invalid={isInvalid}>
                <FieldLabel htmlFor={field.name}>Restriction</FieldLabel>
                <Select
                  name={field.name}
                  items={restrictions}
                  onOpenChange={(isOpen) => {
                    if (!isOpen) field.handleBlur();
                  }}
                  onValueChange={(value) => {
                    field.handleChange(value ?? "");
                  }}
                  value={field.state.value}
                >
                  <SelectTrigger id={field.name} aria-invalid={isInvalid}>
                    <SelectValue placeholder="Restriction" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectGroup>
                      {restrictions.map((ga) => (
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
        <form.Field
          name="reaction"
          children={(field) => {
            const isInvalid = field.state.meta.isTouched && !field.state.meta.isValid;
            return (
              <Field data-invalid={isInvalid}>
                <FieldLabel htmlFor={field.name}>Reaction</FieldLabel>
                <Textarea
                  id={field.name}
                  name={field.name}
                  value={field.state.value}
                  onBlur={field.handleBlur}
                  onChange={(e) => {
                    field.handleChange(e.target.value);
                  }}
                  aria-invalid={isInvalid}
                  autoComplete="off"
                  className="resize-none"
                />
                {isInvalid && <FieldError errors={field.state.meta.errors} />}
              </Field>
            );
          }}
        />
        <form.Field
          name="declaration"
          children={(field) => {
            const isInvalid = field.state.meta.isTouched && !field.state.meta.isValid;
            return (
              <Field data-invalid={isInvalid}>
                <FieldLabel htmlFor={field.name}>Declaration</FieldLabel>
                <Textarea
                  id={field.name}
                  name={field.name}
                  value={field.state.value}
                  onBlur={field.handleBlur}
                  onChange={(e) => {
                    field.handleChange(e.target.value);
                  }}
                  aria-invalid={isInvalid}
                  autoComplete="off"
                  className="resize-none"
                />
                {isInvalid && <FieldError errors={field.state.meta.errors} />}
              </Field>
            );
          }}
        />
        <form.Field
          name="effect"
          children={(field) => {
            const isInvalid = field.state.meta.isTouched && !field.state.meta.isValid;
            return (
              <Field data-invalid={isInvalid}>
                <FieldLabel htmlFor={field.name}>Effect</FieldLabel>
                <Textarea
                  id={field.name}
                  name={field.name}
                  value={field.state.value}
                  onBlur={field.handleBlur}
                  onChange={(e) => {
                    field.handleChange(e.target.value);
                  }}
                  aria-invalid={isInvalid}
                  autoComplete="off"
                  className="resize-none"
                />
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
