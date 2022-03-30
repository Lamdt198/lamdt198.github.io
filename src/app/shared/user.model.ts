 
export class User {
    ID: string;
    UserName: string;
    PhoneNumber: string;
    Email: string;
    Password: string;
    IsDelete: number;
    Gendel: number;
    BirthDate: string;
    BirthDateConvert: string;
    CreatedDate: string;
    DeletedDate: string;
    UpdatedDate: string;
  constructor() {
      this.ID = "";
      this.UserName = "";
      this.PhoneNumber = "";
      this.Email = "";
      this.Password = "";
      this.IsDelete = 0;
      this.Gendel = 0;
      this.BirthDate = "";
      this.BirthDateConvert = "";
      this.CreatedDate = "";
      this.DeletedDate = "";
      this.UpdatedDate = "";

    }
  }