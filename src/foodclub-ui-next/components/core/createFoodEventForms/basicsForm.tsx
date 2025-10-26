import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { ImageDropzone } from "../imageDropzone";
import ComponentsWrapper from "../wrappers/componentsWrapper";
import CreateEventFormInput from "./createEventFormInput";
import CreateEventFormTextarea from "./createEventFormTextarea";
import MealCard from "../meal-card/meal-card";
import { EventDetails } from "@/types/event-details.type";


export default function BasicsForm() {
    let eventDetail: EventDetails = {
        id: "5d92825a-133e-4ac8-8fa5-da8696a486a8",
        title: "Noodles & More at Vesterport",
        description: "Join us for a delightful evening of noodles and more!",
        date: "2024-06-15",
        time: "19:00",
        hostName: "Anna S.",
        hostRating: 4.8,
        tags: ["Vegetarian", "Gluten-Free"],
        seatsAvailable: 5,
        totalSeats: 7,
        price: 55,
        imageUrl:
            "https://i0.wp.com/blog.themalamarket.com/wp-content/uploads/2024/06/Vegetarian-pulled-noodles-lead-more-sat.jpg?resize=1200%2C900&ssl=1",
    };

    return (
        // md:grid-cols-[2fr_1fr]
        <div id="mainWrapper" className="grid grid-cols-1 md:grid-cols-[1fr_auto] gap-10">

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
                <MealCard key={123} {...eventDetail} />
            </ComponentsWrapper>


        </div>
    );
}