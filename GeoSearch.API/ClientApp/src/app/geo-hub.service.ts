import { Injectable, Inject } from '@angular/core';
import * as signalR from "@microsoft/signalr";
import { from, Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GeoHubService {
  private hubConnection: signalR.HubConnection;
  private coordinatesResult$: Subject<number> = new Subject<number>();

  constructor(@Inject('BASE_URL') baseUrl: string) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${baseUrl}geo`)
      .build();

    this.hubConnection.on('GeoCountUpdate', (data) => {
        this.coordinatesResult$.next(data);
    });
  }

  public getCoordinatesResult(): Observable<number>{
    return this.coordinatesResult$.asObservable();
  }

  public startConnection(): Promise<void> {
    return this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err))
  }

  public getConnectionId(): Observable<string> {
    return from(this.hubConnection.invoke('GetConnectionId'))
  }
  
  ngOnDestroy(){
    this.hubConnection.stop();
  }

}