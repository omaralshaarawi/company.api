export interface fingerprint {
    fingerprintId : number;
    employeeId : number;
    fingerIndex : string;
    deviceId: string;
    enrolledDate : string | null;
    quality: string | null;
}

export interface createFingerprintRequest {
    employeeId : number;
    fingerIndex : string;
    deviceId: string;
    enrolledDate : string | null;
    quality : string | null;
    templateData: string;
}


