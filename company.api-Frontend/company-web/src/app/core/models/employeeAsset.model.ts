export interface EmployeeAsset {
    employeeAssetId: number;
    employeeId: number;
    assetId: number;
    assignedDate: string; // ISO date string
    returnedDate: string | null; // ISO date string or null if not returned
    notes: string | null;
}

export interface CreateEmployeeAssetRequest {
  employeeId: number;
  assetId: number;
  assignedDate?: string;
  returnDate?: string | null;
  notes: string | null;
}

export interface EmployeeAssetFormModel {
  assetId: string;
  notes: string;
}