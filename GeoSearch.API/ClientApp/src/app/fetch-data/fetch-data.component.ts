import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FileUploader} from 'ng2-file-upload';
import { FormGroup, FormControl, Validators, AbstractControl } from '@angular/forms';
import { GeoHubService } from '../geoHub.service';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public hasBaseDropZoneOver:boolean = false;
  public uploader: FileUploader = new FileUploader({url: ''});
  private hubConnectionId: number;
  private countResult: number = 0;
  private geoDataForm: FormGroup;
  private numericNumberReg = '^-?[0-9]\\d*(\\.\\d{1,2})?$';
  

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, public signalRService: GeoHubService) {
    this.uploader = new FileUploader({url: baseUrl + 'geodata/coordinatesInRange/processFile'});

    this.uploader.onBuildItemForm = (fileItem: any, form: any) => {
      form.append('centerLongitude', this.geoDataForm.get('centerLongitude').value || '');
      form.append('centerLatitude', this.geoDataForm.get('centerLatitude').value || '');
      form.append('borderLongitude' , this.geoDataForm.get('borderLongitude').value || '');
      form.append('borderLatitude' , this.geoDataForm.get('borderLatitude').value || '');
      form.append('hubConnectionId' , this.hubConnectionId);
    };
  }

  ngOnInit(){
    this.geoDataForm = new FormGroup({
      centerLongitude: new FormControl(null, [Validators.required, Validators.min(-180), Validators.max(180), Validators.pattern(this.numericNumberReg)]),
      centerLatitude: new FormControl(null, [Validators.required, Validators.min(-90), Validators.max(90), Validators.pattern(this.numericNumberReg)]),
      borderLongitude: new FormControl(null, [Validators.required, Validators.min(-180), Validators.max(180), Validators.pattern(this.numericNumberReg)]),
      borderLatitude: new FormControl(null, [Validators.required, Validators.min(-90), Validators.max(90), Validators.pattern(this.numericNumberReg)])
    });

    this.signalRService.startConnection()
      .then(x => this.signalRService.getConnectionId()
      .subscribe(id => this.hubConnectionId = id));

    this.signalRService.getCoordinatesResult()
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

  public upload(item: any){
    item.upload();
  }

  public fileOverBase(e:any):void {
    this.hasBaseDropZoneOver = e;
  }

}