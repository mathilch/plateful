"use client"

import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogHeader,
    DialogTitle} from "@/components/ui/dialog"
import { Label } from "../ui/label";
import { Input } from "../ui/input";
import { Button } from "../ui/button";

type AuthDialogProps = {
    open: boolean;
    setOpenAction: (open: boolean) => void;
};

export default function AuthDialog({ open, setOpenAction: setOpen }: AuthDialogProps) {
    //const [open, setOpen] = useState(false);    

    return (
        <Dialog open={open} onOpenChange={setOpen} >
            <DialogContent>
                <DialogHeader>
                    <DialogTitle>Login to your account</DialogTitle>
                    <DialogDescription>
                        Enter your email below to login to your account
                    </DialogDescription>
                    {/* <CardAction>
          <Button variant="link">Sign Up</Button>
        </CardAction> */}
                </DialogHeader>
                <form>
                    <div className="flex flex-col gap-6">
                        <div className="grid gap-2">
                            <Label htmlFor="email" className="justify-self-center">Email</Label>
                            <Input
                                id="email"
                                type="email"
                                placeholder="m@example.com"
                                required
                            />
                        </div>
                        <div className="grid gap-2">
                            <div className="grid grid-cols-3 gap-4 ">
                                <div className=""></div>

                                <Label htmlFor="password" className="justify-self-center">Password</Label>

                         
                            </div>

                            <Input id="password" type="password" required />

                                   <a
                                    href="#"
                                    className="text-sm justify-self-end underline-offset-4 hover:underline"
                                >
                                    Forgot password?
                                </a>
                        </div>
                    </div>
                </form>

                {/* <DialogFooter className="flex-col gap-2"> */}
                    <Button type="submit" className=" bg-amber-500">
                        Login
                    </Button>
                    <Button variant="outline" className="">
                        Login with Google
                    </Button>

                    {/* <Separator /> */}
                    <p className="justify-self-center">Or</p>

                    <Button variant="outline" className="">Sign Up</Button>


                {/* </DialogFooter> */}

            </DialogContent>
        </Dialog>

    )
}