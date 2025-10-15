"use client"

import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogHeader,
    DialogTitle
} from "@/components/ui/dialog"
import { Label } from "../ui/label";
import { Input } from "../ui/input";
import { Button } from "../ui/button";

type SignUpDialogProps = {
    open: boolean;
    setOpenAction: (open: boolean) => void;
};

export default function SignUpDialog({ open, setOpenAction: setOpen }: SignUpDialogProps) {
    //const [open, setOpen] = useState(false);    

    return (
        <Dialog open={open} onOpenChange={setOpen} >
            <DialogContent className="p-10">

                <DialogHeader>
                    <DialogTitle className="text-primary-gray text-3xl font-bold font-['Poppins']">Create your account</DialogTitle>
                    <DialogDescription>
                        Join dinners or host your own.
                    </DialogDescription>

                </DialogHeader>

                <form className="grid gap-4">

                    <div className="grid gap-2">
                        <Label htmlFor="fullName" className="text-xs text-muted-gray font-bold">Full Name</Label>
                        <Input
                            id="fullName"
                            type="text"
                            placeholder="Enter your full name"
                            required
                            className="h-12"
                        />
                    </div>

                    <div className="grid gap-2">
                        <Label htmlFor="email" className="text-xs text-muted-gray font-bold">Email</Label>
                        <Input
                            id="email"
                            type="email"
                            placeholder="m@example.com"
                            required
                            className="h-12"
                        />
                    </div>

                    <div className="grid gap-2">
                        <Label htmlFor="age" className="text-xs text-muted-gray font-bold">Full Name</Label>
                        <Input
                            id="age"
                            type="number"
                            placeholder="Enter your age"
                            required
                            className="h-12"
                        />
                    </div>

                    <div className="grid gap-2">
                        <Label htmlFor="password" className="text-xs text-muted-gray font-bold">Password</Label>
                        <Input
                            id="password"
                            type="password"
                            placeholder="8+ characters"
                            required
                            className="h-12" />
                    </div>


                    <div className="grid gap-2">
                        <Label htmlFor="confirmPassword" className="text-xs text-muted-gray font-bold">Confirm password</Label>
                        <Input
                            id="confirmPassword"
                            type="password"
                            placeholder="Repeat password"
                            required
                            className="h-12" />
                    </div>


                    {/* <DialogFooter className="flex-col gap-2"> */}
                    <Button type="submit" className="h-9 bg-primary-green font-bold cursor-pointer">
                        Sign Up
                    </Button>

                </form>

                <p className="justify-self-center text-xs text-muted-gray">By continuing, you agree to our Terms and Privacy.</p>

                <a
                    href="#"
                    className="text-xs font-bold text-primary-green justify-self-end underline-offset-4 hover:underline"
                >
                    Already have an account? Log in
                </a>


            </DialogContent>
        </Dialog>

    )
}