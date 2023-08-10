export class ApiError extends Error {
    constructor(
        public statusCode: number,
        public title: string,
        public errorMessages: string[]
    ) {
        super();
    }
}

export type Result<TVal, TError> = {
    hasError: true;
    error: TError;
} | {
    hasError: false;
    value: TVal;
}

