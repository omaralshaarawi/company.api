// core/services/notification.service.ts
import { Injectable, signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../environments/environment';

export interface AssetAssignedEvent {
    assetId: number; employeeId: number; assignedDate: string;
}

export interface AssetReturnedEvent {
    assetId: number; employeeId: number; returnedDate: string;
}

export interface ReportGeneratedEvent {
    reportId: number;
     title: string;
    relatedEmployeeId: number; 
}
@Injectable({ providedIn: 'root' })
export class NotificationService {
    private connection: signalR.HubConnection | null = null;

    // Signals the rest of the app can react to — updated every time a message arrives
    readonly lastAssetAssigned = signal<AssetAssignedEvent | null>(null);
    readonly lastAssetReturned = signal<AssetReturnedEvent | null>(null);
    readonly lastReportGenerated = signal<ReportGeneratedEvent | null>(null);
    readonly connectionState = signal<signalR.HubConnectionState>(
        signalR.HubConnectionState.Disconnected
    );

    connect(): void {
        if (this.connection) return; // already connecting/connected

        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(`${environment.apiUrl.replace('/api', '')}/hubs/notifications`, {
                accessTokenFactory: () => localStorage.getItem('auth_token') ?? ''
            })
            .withAutomaticReconnect()
            .configureLogging(signalR.LogLevel.Debug)
            .build();
        this.connection.on('AssetAssigned', (data: AssetAssignedEvent) => {
            this.lastAssetAssigned.set(data);
        });
        this.connection.on('AssetReturned', (data: AssetReturnedEvent) => {
            this.lastAssetReturned.set(data);
        });
        this.connection.on('ReportGenerated', (data: ReportGeneratedEvent) => {
            this.lastReportGenerated.set(data);
        });
        this.connection.onreconnecting(() =>
            this.connectionState.set(signalR.HubConnectionState.Reconnecting));
        this.connection.onreconnected(() =>
            this.connectionState.set(signalR.HubConnectionState.Connected));
        this.connection.onclose(() =>
            this.connectionState.set(signalR.HubConnectionState.Disconnected));
        this.connectionState.set(signalR.HubConnectionState.Connecting);
        this.connection.start()
            .then(() => this.connectionState.set(signalR.HubConnectionState.Connected))
            .catch(err => {
            console.error('SignalR connection failed:', err);
            this.connection = null; // allow a future connect() to retry
            this.connectionState.set(signalR.HubConnectionState.Disconnected);
        });
    }
    disconnect(): void {
        this.connection?.stop();
        this.connection = null;
        this.connectionState.set(signalR.HubConnectionState.Disconnected);
    }
}