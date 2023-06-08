interface CreateProfileDto{
	firstName: string;
	middleName: string;
	lastName: string;
	phoneNumber: string;
	userImage: string | null;
	birthDate: string;
	gender: "male" | "female";
}

export interface ChangeUserDto extends CreateProfileDto
{
	identityId: string;
	isActive: boolean;
}

export default CreateProfileDto