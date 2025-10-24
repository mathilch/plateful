import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import * as React from "react"

interface CreateEventFormInputProps extends React.InputHTMLAttributes<HTMLInputElement> {
  labelText: string,
  id: string
}

export default function CreateEventFormInput({
  labelText: label,
  id,
  ...props
}: CreateEventFormInputProps) {
  return (
    <div className="grid gap-2">
      <Label
        htmlFor={id}
        className="text-xs font-bold text-muted-foreground"
      >
        {label}
      </Label>

      <Input
        id={id}
        className="h-10 bg-gray-50 text-sm font-normal text-muted-foreground font-['Poppins']"
        {...props}
      />
    </div>
  )
}
