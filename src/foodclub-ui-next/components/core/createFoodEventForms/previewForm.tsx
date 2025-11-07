"use client";

import { useFormWizardContext } from "./formWizardContext";


export default function PreviewForm() {
    const [formState, formDispatch] = useFormWizardContext();


      function onSubmit(e: React.FormEvent) {
        e.preventDefault();
    }

    return (
        <form>
            <p>This is the step5.</p>
                 <input type="submit" onSubmit={onSubmit}/>
        </form>
    );
}