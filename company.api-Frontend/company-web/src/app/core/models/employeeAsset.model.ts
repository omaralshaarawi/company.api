export interface employeeAsset {
    employeeAssetId: number;
    employeeId: number;
    assetId: number;
    assignedDate: string; // ISO date string
    returnedDate: string | null; // ISO date string or null if not returned
    notes: string | null;
}