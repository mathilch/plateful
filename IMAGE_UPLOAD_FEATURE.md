# Image Upload Feature Implementation

## Overview
This document describes the implementation of the image upload feature for event creation. Previously, images were converted to base64 strings and stored directly in the database, which was inefficient and didn't work properly. The new implementation uploads images to the local disk and stores only the file path/URL in the database.

## Changes Made

### Backend (.NET/C# - Events API)

#### 1. New File Upload Endpoint
**File**: `src/EventsMicroservice/src/Events.Api/Controllers/EventController.cs`

Added a new endpoint `POST /api/event/upload-image` that:
- Accepts multipart/form-data file uploads
- Validates file type (only images: .jpg, .jpeg, .png, .gif, .webp)
- Validates file size (max 5MB)
- Generates unique filenames using GUIDs to prevent collisions
- Saves files to `wwwroot/uploads/` directory
- Returns the relative URL to access the uploaded image

**Example Response**:
```json
{
  "imageUrl": "/uploads/a1b2c3d4-e5f6-7890-abcd-ef1234567890.jpg"
}
```

#### 2. Static File Serving
**File**: `src/EventsMicroservice/src/Events.Api/Program.cs`

Added `app.UseStaticFiles()` middleware to enable serving uploaded images from the `wwwroot` directory. This makes uploaded images publicly accessible at URLs like:
```
http://localhost:5002/uploads/{filename}
```

#### 3. Upload Directory Structure
Created `src/EventsMicroservice/src/Events.Api/wwwroot/uploads/` directory with a `.gitkeep` file to ensure the directory exists in version control while keeping uploaded files out of git.

### Frontend (Next.js/TypeScript)

#### 1. ImageDropzone Component
**File**: `src/foodclub-ui-next/components/core/imageDropzone.tsx`

Updated the component to:
- Upload files immediately when dropped/selected (instead of converting to base64)
- Show uploading state with visual feedback
- Display error messages if upload fails
- Store the full image URL (returned from the API) in the form state
- Disable the dropzone during upload to prevent concurrent uploads

**Key Changes**:
- Removed `FileReader` usage for base64 conversion
- Added `fetch` call to upload endpoint with FormData
- Added loading and error states
- Properly handle authentication with Bearer token

#### 2. Form Integration
**File**: `src/foodclub-ui-next/components/core/createFoodEventForms/previewForm.tsx`

No changes needed - the form already uses `formState.basics?.coverImage` which now contains the full URL instead of base64 data.

### Configuration

#### 1. .gitignore
**File**: `.gitignore`

Added patterns to ignore uploaded files:
```
# Uploaded images (don't commit user-uploaded content)
src/EventsMicroservice/src/Events.Api/wwwroot/uploads/*
!src/EventsMicroservice/src/Events.Api/wwwroot/uploads/.gitkeep
```

## How It Works

### Upload Flow
1. User drops/selects an image in the ImageDropzone component
2. Component immediately uploads the file to `POST /api/event/upload-image`
3. Backend validates the file and saves it to disk with a unique filename
4. Backend returns the relative URL (e.g., `/uploads/{guid}.jpg`)
5. Frontend stores the full URL (e.g., `http://localhost:5002/uploads/{guid}.jpg`)
6. When creating the event, this URL is sent as `imageThumbnail` in the CreateEventRequestDto
7. The URL is stored in the database's `ImageThumbnail` field
8. Frontend can display the image by using the stored URL

### Example Request Flow

**1. Upload Image**:
```http
POST /api/event/upload-image
Content-Type: multipart/form-data
Authorization: Bearer {token}

file: (binary image data)
```

**Response**:
```json
{
  "imageUrl": "/uploads/a1b2c3d4-e5f6-7890-abcd-ef1234567890.jpg"
}
```

**2. Create Event**:
```http
POST /api/event
Content-Type: application/json
Authorization: Bearer {token}

{
  "name": "Dinner Party",
  "description": "A lovely dinner",
  "imageThumbnail": "http://localhost:5002/uploads/a1b2c3d4-e5f6-7890-abcd-ef1234567890.jpg",
  ...
}
```

**3. Display Image**:
```html
<img src="http://localhost:5002/uploads/a1b2c3d4-e5f6-7890-abcd-ef1234567890.jpg" alt="Event" />
```

## Benefits

1. **Reduced Database Size**: Images are stored on disk, not in the database
2. **Better Performance**: Smaller database records, faster queries
3. **Easier Scaling**: Can move uploaded files to CDN or cloud storage later
4. **Standard Approach**: Uses standard file upload patterns
5. **Publicly Accessible**: Images can be accessed via URL by frontend

## Testing

### Manual Testing Steps

1. **Start the application**:
   ```bash
   docker-compose up
   ```

2. **Create an event**:
   - Navigate to the create event page
   - Fill in the event details
   - Drop an image in the cover photo dropzone
   - Observe the upload progress
   - Complete the event creation

3. **Verify**:
   - Check that the uploaded file exists in `src/EventsMicroservice/src/Events.Api/wwwroot/uploads/`
   - View the created event - the image should be displayed
   - Check the database - `ImageThumbnail` should contain a URL, not base64 data

### Automated Tests

All existing E2E tests pass (82/82 tests):
```bash
cd src/EventsMicroservice
dotnet test test/Events.Api.E2ETests/Events.Api.E2ETests.csproj
```

The tests validate that:
- Event creation still works with string values for `ImageThumbnail`
- Validation rules (max 150 chars) still apply
- All other event fields work correctly

## Security Considerations

1. **File Type Validation**: Only image files are accepted
2. **File Size Limit**: Maximum 5MB per image
3. **Unique Filenames**: GUIDs prevent filename collisions and path traversal
4. **Authentication Required**: Upload endpoint requires Bearer token
5. **No Executable Files**: Only image extensions are allowed

## Future Improvements

1. **Image Optimization**: Resize/compress images on upload
2. **Multiple Images**: Support multiple images per event (already has `EventImages` collection)
3. **Cloud Storage**: Move to Azure Blob Storage or AWS S3 for production
4. **Image Deletion**: Clean up unused images when events are deleted
5. **Thumbnails**: Generate different sizes for responsive images
6. **Progress Indicator**: Show upload percentage for large files

## Migration Notes

### For Existing Data
If there are existing events with base64 data in `ImageThumbnail`:
1. The data will remain as-is (backwards compatible)
2. New events will use the new URL format
3. Consider a migration script to extract base64 images to files if needed

### Environment Variables
For production deployment, consider adding:
```
MAX_UPLOAD_SIZE_MB=5
ALLOWED_IMAGE_EXTENSIONS=.jpg,.jpeg,.png,.gif,.webp
UPLOADS_PATH=/app/wwwroot/uploads
```

## Rollback Plan

If issues arise, to rollback:
1. Revert the frontend changes to use base64 conversion
2. Keep the upload endpoint (it won't be used)
3. Frontend will work as before with base64 strings
