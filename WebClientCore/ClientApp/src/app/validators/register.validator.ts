import { AbstractControl } from "@angular/forms";

const usernameRegex: RegExp = new RegExp("^[a-zA-Z][a-zA-Z0-9]{3,20}$");
//new RegExp("^.*(?=.{6,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).*$");
//1 главна буква, 1 малка, 1 цифра, минимум 6 знака
const passwordRegex: RegExp = new RegExp("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{6,}$");

export class RegisterValidators {

  static usernameValidator(c: AbstractControl): { [key: string]: boolean } | null {
    if (c.value !== null && !usernameRegex.test(c.value)) {
      return { 'usernameformat': true };
    }
    return null;
  }

  static passwordValidator(c: AbstractControl): { [key: string]: boolean } | null {
    if (c.value !== null && !passwordRegex.test(c.value)) {
      return { 'passwordformat': true };
    }
    return null;
  }

  static passwordMatcher(c: AbstractControl): { [key: string]: boolean } | null {
    const passwordControl = c.get("password");
    const confirmPasswordControl = c.get("confirmPassword");

    //If the password or confirmPassword fields have not been touched, return null and skip the validation
    if (passwordControl.pristine || confirmPasswordControl.pristine) {
      return null;
    }

    if (passwordControl.value === confirmPasswordControl.value) {
      return null;
    }
    return { 'match': true };
  }
}
