"use client";

import {useParams, useRouter} from "next/navigation";
import {useEffect, useState} from "react";
import { EventOverviewDto } from "@/types/event-details.type"
import { EventFoodDetails } from "@/types/event-food-details.type";
import { parseJwt } from "@/lib/jwt-decoder.helper";
import { getEventById } from "@/services/api/events-api.service";

export default function EventDetails() {
    
    const { id } = useParams();
    const router = useRouter();
    
    const [event, setEvent] = useState<EventOverviewDto | null>(null);
    const [foodDetails, setFoods] = useState<EventFoodDetails | null>(null);

    useEffect(() => {
        if (!id) return;
        
        const token = localStorage.getItem("accessToken");
        if (!token) {
            router.replace("/");
            return;
        }
        
        try {
            const _ = parseJwt(token);

            (async () => {
                try {
                    const eventData = await getEventById(id as string, token);
                    setEvent(eventData);
                } catch (err) {
                    console.error("Failed to fetch event details:", err);
                }
            })();
        } catch (err) {
            console.error("Invalid token:", err);
        }
    }, [id, router]);
    
    return (
        <div className="centered-event-page">
            "This is a test"
        </div>
    )
    
}