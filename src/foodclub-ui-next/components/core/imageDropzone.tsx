"use client"

import { useCallback, useState } from "react"
import { useDropzone } from "react-dropzone"
import { Label } from "@/components/ui/label"

interface ImageDropzoneProps {
    value?: string;
    onChange?: (imageUrl: string) => void;
}

export function ImageDropzone({ value, onChange }: ImageDropzoneProps) {
    const [uploading, setUploading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const onDrop = useCallback(async (acceptedFiles: File[]) => {
        if (acceptedFiles.length > 0 && onChange) {
            const file = acceptedFiles[0];
            setUploading(true);
            setError(null);

            try {
                const formData = new FormData();
                formData.append('file', file);

                const token = localStorage.getItem('accessToken');
                if (!token) {
                    throw new Error('Authentication required. Please log in.');
                }

                const response = await fetch(`${process.env.NEXT_PUBLIC_EVENTS_API_BASE_URL}/api/event/upload-image`, {
                    method: 'POST',
                    headers: {
                        'Authorization': `Bearer ${token}`
                    },
                    body: formData
                });

                if (!response.ok) {
                    const errorData = await response.json();
                    throw new Error(errorData.error || 'Failed to upload image');
                }

                const data = await response.json();
                const fullImageUrl = `${process.env.NEXT_PUBLIC_EVENTS_API_BASE_URL}${data.imageUrl}`;
                onChange(fullImageUrl);
            } catch (err) {
                console.error('Upload error:', err);
                setError(err instanceof Error ? err.message : 'Failed to upload image');
            } finally {
                setUploading(false);
            }
        }
    }, [onChange])

    const { getRootProps, getInputProps, isDragActive } = useDropzone({
        onDrop,
        accept: { "image/*": [] },
        disabled: uploading
    })

    return (
        <div>
            <div
                {...getRootProps()}
                className={`flex h-32 w-80 items-center justify-center rounded-xl border border-dashed border-gray-300 bg-muted text-sm text-muted-foreground transition hover:bg-muted/80 ${
                    isDragActive ? "border-primary bg-muted/50" : ""
                } ${uploading ? "opacity-50 cursor-not-allowed" : ""}`}
            >
                <input {...getInputProps()} />
                {uploading ? (
                    <span>Uploading...</span>
                ) : value ? (
                    <img src={value} alt="Preview" className="h-full w-full object-cover rounded-xl" />
                ) : (
                    <span>{isDragActive ? "Drop the image here..." : "Drop image"}</span>
                )}
            </div>
            {error && (
                <p className="text-xs text-red-600 mt-1">{error}</p>
            )}
        </div>
    )
}
