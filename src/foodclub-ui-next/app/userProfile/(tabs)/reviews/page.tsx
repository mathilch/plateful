"use client";

export default function Reviews() {
  return (
    <div>
      <h2 className="text-xl font-bold mb-6">Reviews</h2>
      
      <div className="space-y-6">
        {[1, 2, 3].map((num) => (
          <div key={num} className="bg-white rounded-lg p-6">
            <div className="flex items-center gap-4 mb-4">
              <div className="w-12 h-12 rounded-full bg-gray-100"></div>
              <div>
                <h3 className="font-semibold">Guest Name</h3>
                <p className="text-gray-600 text-sm">October 1, 2025</p>
              </div>
            </div>
            <div className="flex gap-1 text-yellow-400 mb-2">
              {'★'.repeat(5)}
            </div>
            <p className="text-gray-700">
              Great host! The food was amazing and the atmosphere was very welcoming. 
              Would definitely recommend this experience to others.
            </p>
          </div>
        ))}
      </div>
    </div>
  );
}