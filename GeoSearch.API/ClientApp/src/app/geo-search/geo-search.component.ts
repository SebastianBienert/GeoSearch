import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { FileUploader, FileItem} from 'ng2-file-upload';
import { FormGroup, FormControl, Validators, AbstractControl } from '@angular/forms';
import { GeoHubService } from '../geo-hub.service';
import { ReplaySubject } from 'rxjs';
import { take, takeUntil} from 'rxjs/operators';

@Component({
  selector: 'geo-search',
  templateUrl: './geo-search.component.html',
  styleUrls: ['./geo-search.component.css']
})
export class GeoSearchComponent implements OnInit, OnDestroy {
  public hasBaseDropZoneOver: boolean = false;
  public uploader: FileUploader;
  public countResult: number = 0;
  public geoDataForm: FormGroup;
  private hubConnectionId: string;
  private numericNumberReg = '^-?[0-9]\\d*(\\.\\d{1,2})?$';
  private destroyed$: ReplaySubject<boolean> = new ReplaySubject(1);
  
  constructor(@Inject('BASE_URL') baseUrl: string, public signalRService: GeoHubService) {
    this.uploader = new FileUploader({url: baseUrl + 'geodata/coordinatesInRange/processFile'});

    this.uploader.onBuildItemForm = (fileItem: FileItem, form: any) => {
      form.append('centerLongitude', this.geoDataForm.get('centerLongitude').value || '');
      form.append('centerLatitude', this.geoDataForm.get('centerLatitude').value || '');
      form.append('borderLongitude' , this.geoDataForm.get('borderLongitude').value || '');
      form.append('borderLatitude' , this.geoDataForm.get('borderLatitude').value || '');
      form.append('hubConnectionId' , this.hubConnectionId);
    };

    this.uploader.onSuccessItem = (item: FileItem, response: string) => {
      item.headers.result = Number(response);
    };
  }

  ngOnInit(){
    ($('[data-toggle="popover"]') as any).popover();
    this.geoDataForm = new FormGroup({
      centerLongitude: new FormControl(-119, [Validators.required, Validators.min(-180), Validators.max(180), Validators.pattern(this.numericNumberReg)]),
      centerLatitude: new FormControl(35, [Validators.required, Validators.min(-90), Validators.max(90), Validators.pattern(this.numericNumberReg)]),
      borderLongitude: new FormControl(-122, [Validators.required, Validators.min(-180), Validators.max(180), Validators.pattern(this.numericNumberReg)]),
      borderLatitude: new FormControl(40, [Validators.required, Validators.min(-90), Validators.max(90), Validators.pattern(this.numericNumberReg)])
    });

    this.signalRService.startConnection()
      .then(_ => this.signalRService.getConnectionId()
        .pipe(take(1))
        .subscribe(id => this.hubConnectionId = id)
      );

    this.signalRService.getCoordinatesResult()
      .pipe(takeUntil(this.destroyed$))
      .subscribe(x => this.countResult = x);
  }

  get centerLongitude(): AbstractControl {
    return this.geoDataForm.get('centerLongitude');
  }

  get centerLatitude(): AbstractControl {
    return this.geoDataForm.get('centerLatitude');
  }

  get borderLongitude(): AbstractControl {
    return this.geoDataForm.get('borderLongitude');
  }

  get borderLatitude(): AbstractControl {
    return this.geoDataForm.get('borderLatitude');
  }

  get currentFileItem(): FileItem | null{
    const queueLength = this.uploader.queue.length;
    return queueLength == 0 ? null : this.uploader.queue[queueLength - 1];
  }

  public upload(item: any): void{
    this.countResult = 0;
    item.upload();
  }

  public cancel(item: any): void{
    item.cancel();
  }

  public fileOverBase(e:any): void{
    this.hasBaseDropZoneOver = e;
  }

  ngOnDestroy(){
    this.destroyed$.next(true);
    this.destroyed$.complete();
  }
}