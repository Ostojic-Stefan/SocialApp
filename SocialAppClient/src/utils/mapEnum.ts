export function getEnumValues(enumeration: Record<any, any>) {
    return Object.values(enumeration)
        .filter(v => !isNaN(Number(v)));
}