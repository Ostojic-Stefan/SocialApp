import { useAppDispatch } from "../../../store";
import UploadImageForm from "../../../ui/ImageUploadForm/UploadImageForm"
import { uploadProfileImage } from "../userProfileSlice";

function UploadUserProfileForm() {
    const dispatch = useAppDispatch();
    async function handleSubmit(formData: FormData): Promise<void> {
        await dispatch(uploadProfileImage(formData));
        // const result = await userService.uploadProfileImage(formData);
        // if (result.hasError) {
        //     failureToast("Failed to upload profile image")
        //     return;
        // }

        // const avatarUrl = result.value;

        // const setProfileImageResult = await userService.setProfileImage(avatarUrl);
        // if (setProfileImageResult.hasError) {
        //     failureToast("Failed to set the uploaded profile image");
        //     return;
        // }
        // successToast("Successfully uploaded profile image");
    }

    return (
        <UploadImageForm onSubmit={handleSubmit} />
    );
}

export default UploadUserProfileForm
