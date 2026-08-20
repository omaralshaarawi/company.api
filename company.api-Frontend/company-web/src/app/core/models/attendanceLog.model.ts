export interface AttendanceLog {
	attendanceLogId: number;
	employeeId: number;
	deviceId: string;
	eventType: 'CheckIn' | 'CheckOut' | string;
	eventTime: string;
}
