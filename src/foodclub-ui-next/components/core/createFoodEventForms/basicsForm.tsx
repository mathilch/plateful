import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { ImageDropzone } from "../imageDropzone";
import ComponentsWrapper from "./componentsWrapper";
import CreateEventFormInput from "./createEventFormInput";
import CreateEventFormTextarea from "./createEventFormTextarea";


export default function BasicsForm() {
    return (
        <div id="mainWrapper" className="grid grid-cols-1 md:grid-cols-[2fr_1fr] gap-10">

            <ComponentsWrapper id="basicsForm">
                {/* <div id="basicsForm" className="border-1 border-gray-200 p-8 grid gap-4 rounded-xl"> */}

                <CreateEventFormInput
                    id="mealTitle"
                    labelText="Meal Title"
                    type="text"
                    placeholder="e.g., Cozy Curry Night"
                    required
                />

                <CreateEventFormTextarea
                    id="descriptionTextArea"
                    labelText="Description"
                    required
                    placeholder="Describe your menu, vibe, BYOB, etc."
                    className="h-48 bg-gray-50 text-sm font-normal text-[#9CA3AF] font-['Poppins']"
                />



                <div className="grid gap-2">
                    <Label htmlFor="coverPhotoDropzone" className="text-xs text-muted-gray font-bold">Cover Photo</Label>
                    <ImageDropzone />
                </div>


                {/* </div> */}
            </ComponentsWrapper>


            <ComponentsWrapper id="livePreview">
                <h3>Live Preview</h3>
            </ComponentsWrapper>


        </div>
    );
}