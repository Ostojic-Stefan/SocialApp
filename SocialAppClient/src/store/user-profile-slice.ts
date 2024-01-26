import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { AddUserImageRequest, GetUserImagesRequest, SetProfileImageRequest, UserDetailsResponse, UserProfileInformationRequest } from "../api/dtos/user";
import { ApiError } from "../api/models";
import { userService } from "../api/userService";
import { ImageResponse } from "../api/dtos/image";

interface StateType {
    userProfile: UserDetailsResponse | undefined;
    imagesForUser: ImageResponse[];
}

const initialState: StateType = {
    imagesForUser: [],
    userProfile: undefined
}

export const getUserProfileInformation = createAsyncThunk
    <UserDetailsResponse, UserProfileInformationRequest, { rejectValue: ApiError }>(
        'userProfiles/getInformation', async function (request, { rejectWithValue }) {
            const res = await userService.getUserProfileInformation(request);
            if (res.hasError) {
                return rejectWithValue(res.error);
            }
            return res.value;
        }
    );

export const setUserProfileImage = createAsyncThunk<string, SetProfileImageRequest, { rejectValue: ApiError }>(
    "userProfiles/setProfileImage", async function (request, { rejectWithValue }) {
        const res = await userService.setProfileImage({ imageId: request.imageId })
        if (res.hasError) {
            return rejectWithValue(res.error);
        }
        return request.imageId;
    }
)

export const getUserProfileImages = createAsyncThunk<ImageResponse[], GetUserImagesRequest, { rejectValue: ApiError }>(
    "userProfiles/getImages", async function (request, { rejectWithValue }) {
        const res = await userService.getUserImages(request);
        if (res.hasError) {
            return rejectWithValue(res.error);
        }
        return res.value;
    }
)

export const addUserImage = createAsyncThunk<boolean, AddUserImageRequest, { rejectValue: ApiError }>(
    "userProfiles/addUserImage", async function (request, { rejectWithValue }) {
        const res = await userService.addUserImage(request);
        if (res.hasError) {
            return rejectWithValue(res.error);
        }
        return res.value;
    }
)

const userProfileSlice = createSlice({
    name: 'userProfile',
    initialState,
    reducers: {},
    extraReducers: function (builder) {
        builder.addCase(getUserProfileInformation.fulfilled, (state, action) => {
            state.userProfile = action.payload;
        }).addCase(getUserProfileInformation.rejected, (_state, action) => {
            console.log('Failed to get user profile information');
            console.log(action.payload);
        });

        builder.addCase(getUserProfileImages.fulfilled, (state, action) => {
            state.imagesForUser = action.payload;
        }).addCase(getUserProfileImages.rejected, (_state, action) => {
            console.log('Failed to get user images');
            console.log(action.payload);
        })

        builder.addCase(addUserImage.fulfilled, (state, action) => {
            // TODO:
        }).addCase(addUserImage.rejected, (state, action) => {
            console.log('Failed to add user image');
            console.log(action.payload);
        })

        builder.addCase(setUserProfileImage.fulfilled, (state, action) => {
            console.log('successfully set profile image');
            const image = state.imagesForUser.find(img => img.imageId === action.payload);
            if (!image) {
                console.log('Failed to find image locally');
                return;
            }
            if (state.userProfile) {
                state.userProfile.userInfo.profileImage = image;
            }
        }).addCase(setUserProfileImage.rejected, (_state, action) => {
            console.log('failed to set profile image');
            console.log(action.payload);
        });
    }
})

export default userProfileSlice.reducer;