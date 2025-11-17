"use client"

import { useCallback } from "react"
import { useDropzone } from "react-dropzone"
import { Label } from "@/components/ui/label"

export function ImageDropzone() {
    const onDrop = useCallback((acceptedFiles: File[]) => {
        console.log(acceptedFiles)
    }, [])

    const { getRootProps, getInputProps, isDragActive } = useDropzone({
        onDrop,
        accept: { "image/*": [] },
    })

    return (

        <div
            {...getRootProps()}
            className={`flex h-32 w-80 items-center justify-center rounded-xl border border-dashed border-gray-300 bg-muted text-sm text-muted-foreground transition hover:bg-muted/80 ${isDragActive ? "border-primary bg-muted/50" : ""
                }`}
        >
            <input {...getInputProps()} />
            {isDragActive ? "Drop the image here..." : "Drop image"}
        </div>

    )
}
