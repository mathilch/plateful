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

type LoginDialogProps = {
    open: boolean;
    setOpenAction: (open: boolean) => void;
};

export default function LoginDialog({ open, setOpenAction: setOpen }: LoginDialogProps) {
    //const [open, setOpen] = useState(false);    

    return (
        <Dialog open={open} onOpenChange={setOpen} >
            <DialogContent className="p-10">

                <DialogHeader>
                    <DialogTitle className="text-primary-gray text-3xl font-bold font-['Poppins']">Welcome back</DialogTitle>
                    <DialogDescription>
                        {/* Enter your email below to login to your account */}
                    </DialogDescription>

                </DialogHeader>

                <form className="grid gap-4">
                    
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

                        <Label htmlFor="password" className="text-xs text-muted-gray font-bold">Password</Label>
                        <Input
                            id="password"
                            type="password"
                            placeholder="••••••••"
                            required
                            className="h-12" />

                    </div>


                    {/* <DialogFooter className="flex-col gap-2"> */}
                    <Button type="submit" className="h-9 bg-primary-green font-bold cursor-pointer">
                        Log In
                    </Button>

                </form>

                <a
                    href="#"
                    className="text-xs font-bold text-primary-green justify-self-end underline-offset-4 hover:underline"
                >
                    Forgot password?
                </a>

                <Button variant="outline" className="h-9 cursor-pointer">
                    Login with Google
                </Button>

                {/* <Separator /> */}
                <p className="justify-self-center">Or</p>

                <Button variant="outline" className="h-9 cursor-pointer">Sign Up</Button>


                {/* </DialogFooter> */}

            </DialogContent>
        </Dialog>

    )
}