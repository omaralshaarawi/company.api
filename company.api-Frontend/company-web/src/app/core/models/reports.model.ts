export interface report{
    reportId: number;
    employeeName: string | null;
    reportTypeName: string | null;
    title: string;
    summary : string |  null;
    createdAt: string;
}

export interface createReport{
    reportTypeId: number | null;
    title: string,
    generatedById: number | null;
    relatedEmployeeId: number | null;
    relatedAssetId: number | null;
}