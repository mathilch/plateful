"use client";

import { Elements, PaymentElement, useStripe, useElements } from "@stripe/react-stripe-js";
import { useState } from "react";

interface PaymentModalProps {
    clientSecret: string;
    onClose: () => void;
    onSuccess: () => void;
}


const PaymentModal = ({ clientSecret, onClose, onSuccess }: PaymentModalProps) => {
    const stripe = useStripe();
    const elements = useElements();
    const [loading, setLoading] = useState(false);

    const handlePayment = async () => {
        if (!stripe || !elements) return;

        setLoading(true);
        const { error, paymentIntent } = await stripe.confirmPayment({
            elements,
            confirmParams: {},
            redirect: "if_required",
        });

        setLoading(false);

        if (error) {
            alert(error.message);
            return;
        }

        if (paymentIntent?.status === "succeeded") {
            await onSuccess();
            onClose();
        }
    };

    return (
        <div className="fixed inset-0 bg-black/40 flex items-center justify-center z-50">
            <div className="bg-white p-6 rounded-xl w-full max-w-md shadow-lg">
                <h2 className="text-xl font-semibold mb-4">Complete Payment</h2>

                <PaymentElement />

                <button
                    onClick={handlePayment}
                    disabled={loading}
                    className="w-full mt-4 bg-green-700 hover:bg-green-800 text-white py-3 rounded-lg"
                >
                    {loading ? "Processing..." : "Pay now"}
                </button>

                <button
                    onClick={onClose}
                    className="w-full mt-2 border py-3 rounded-lg"
                >
                    Cancel
                </button>
            </div>
        </div>
    );
}

export default PaymentModal;