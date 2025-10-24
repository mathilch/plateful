import { cn } from "@/lib/utils";


export default function ComponentsWrapper({ className, ...props }: React.ComponentProps<"div">) {


    return (
        <div
            className={cn("border-1 border-gray-200 p-8 grid gap-4 rounded-xl", className)}
            {...props}
        />

    );

}