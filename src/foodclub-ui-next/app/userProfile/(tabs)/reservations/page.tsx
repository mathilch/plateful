"use client";

import { Button } from "@/components/ui/button";

export default function Reservations() {
  return (
    <div>
      <h2 className="text-xl font-bold mb-6">My Reservations</h2>
      
      <div className="space-y-4">
        {[1, 2].map((num) => (
          <div key={num} className="bg-white rounded-lg p-4 flex items-center gap-4">
            <div className="w-20 h-20 bg-gray-100 rounded-lg"></div>
            
            <div className="flex-1">
              <h3 className="font-semibold mb-1">Community Dinner #{num}</h3>
              <p className="text-gray-600 text-sm">
                Nørrebro • Fri, 15 Oct • 18:30 • DKK 55
              </p>
              <p className="text-emerald-600 text-sm mt-1">Confirmed • 2 guests</p>
            </div>

            <Button variant="outline" className="border-emerald-800 text-emerald-800 hover:bg-emerald-50">
              View Details
            </Button>
          </div>
        ))}
      </div>
    </div>
  );
}