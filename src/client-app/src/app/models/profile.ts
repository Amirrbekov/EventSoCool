import { User } from "./user";

// eslint-disable-next-line @typescript-eslint/no-unsafe-declaration-merging
export interface Profile {
    username: string;
    displayName: string;
    image?: string;
    bio?: string;
    followersCount: number;
    followingCount: number;
    following: boolean;
    photos?: Photo[]
}

// eslint-disable-next-line @typescript-eslint/no-unsafe-declaration-merging
export class Profile implements Profile {
    constructor(user: User) {
        this.username = user.username;
        this.displayName = user.displayName;
        this.image = user.image
    }
}

export interface Photo {
    id: string;
    url: string;
    isMain: boolean;
}

export interface UserActivity {
    id: string;
    title: string;
    category: string;
    date: Date;
}