import Image from "next/image";
import { Button } from "@/components/ui/button"
import {
  Card,
  CardAction,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Separator } from "@/components/ui/separator"
import FoodAppHeader from "@/components/core/header";
import { Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog";

export default function Home() {
  return (
    <div>
      <FoodAppHeader />

      {/* <Dialog>
        <DialogTrigger asChild>
          <Button>Open</Button>
        </DialogTrigger>

        <DialogContent>
          <DialogHeader>
            <DialogTitle>Are you absolutely sure?</DialogTitle>
            <DialogDescription>
              This action cannot be undone. This will permanently delete your account
              and remove your data from our servers.
            </DialogDescription>
          </DialogHeader>
        </DialogContent>
      </Dialog> */}

      <Card className="w-full max-w-sm">
        <CardHeader>
          <CardTitle>Login to your account</CardTitle>
          <CardDescription>
            Enter your email below to login to your account
          </CardDescription>
          {/* <CardAction>
          <Button variant="link">Sign Up</Button>
        </CardAction> */}
        </CardHeader>
        <CardContent>
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

                  <a
                    href="#"
                    className="text-sm justify-self-end underline-offset-4 hover:underline"
                  >
                    Forgot?
                  </a>
                </div>

                <Input id="password" type="password" required />
              </div>
            </div>
          </form>
        </CardContent>
        <CardFooter className="flex-col gap-2">
          <Button type="submit" className="w-full bg-amber-500">
            Login
          </Button>
          <Button variant="outline" className="w-full">
            Login with Google
          </Button>

          {/* <Separator /> */}
          <p>Or</p>

          <CardAction className="w-full">
            <Button variant="outline" className="w-full">Sign Up</Button>
          </CardAction>

        </CardFooter>
      </Card>

    </div>
  );
}
