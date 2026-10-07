import { schema, required, maxLength, } from '@angular/forms/signals';
import { LoginRequest } from '../../../core/models/auth.model';
export const loginSchema = schema<LoginRequest>((path) => {
    required(path.username, { message: $localize`Username is required.` });
    maxLength(path.username, 150);
    required(path.password, { message: $localize`Password is required.` });
});