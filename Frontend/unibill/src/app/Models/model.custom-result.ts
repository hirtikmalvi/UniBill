export interface CustomResult<T> {
  success: boolean;
  message: string;
  errors: string[] | null;
  data: T | null;
}
