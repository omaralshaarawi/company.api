export interface department {
    departmentId: number;
    name: string;
    createdAt: string; // ISO date string
}

export interface CreateDepartmentRequest {
    name: string;
}