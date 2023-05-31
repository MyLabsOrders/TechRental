import { IProduct } from "../product";

interface IUser{
	id: string;
	firstName: string;
	middleName: string;
	lastName: string;
	image: string;
	birthDate: string;
	gender: "male" | "female";
	number: string;
	money: number;
	orders: IProduct[];
}

export default IUser