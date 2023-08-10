export class ApiError {
    constructor(
        public statusCode: number,
        public title: string,
        public errorMessages: string[]
    ) { }
}

export type Result<TVal, TError> = {
    hasError: true;
    error: TError;
} | {
    hasError: false;
    value: TVal;
}

