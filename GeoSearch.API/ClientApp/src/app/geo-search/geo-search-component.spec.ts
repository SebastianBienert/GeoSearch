import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { GeoSearchComponent } from './geo-search.component';
import { FileUploadModule } from 'ng2-file-upload';
import { GeoHubService } from '../geo-hub.service';
import {getBaseUrl} from '../../injection-tokens';
import { Subject, of } from 'rxjs';

describe('GeoSearchComponent', () => {
    let component: GeoSearchComponent;
    let fixture: ComponentFixture<GeoSearchComponent>;

    const getGeoHubSpy = () => {
        const geoHubSpy = jasmine.createSpyObj('GeoHubService', ['getConnectionId', 'startConnection', 'getCoordinatesResult']);
        geoHubSpy.startConnection.and.returnValue(new Promise<void>(x => x));
        geoHubSpy.getCoordinatesResult.and.returnValue(of(2));
        geoHubSpy.getConnectionId.and.returnValue(of('id0'));

        return geoHubSpy;
    }

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ GeoSearchComponent ],
            imports: [ ReactiveFormsModule, FileUploadModule ],
            providers: [
                {provide: GeoHubService, useValue: getGeoHubSpy()},
                {provide: 'BASE_URL', useFactory: getBaseUrl, deps: []} 
            ]
        })
        .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(GeoSearchComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    const geoCoordinatesTestCases = [
        {inputName: 'centerLongitude', inputValues: ['-180.5', '180.5', null, 'abc'], isValid: false},
        {inputName: 'borderLongitude', inputValues: ['-180.5', '180.5', null, 'abc'], isValid: false},
        {inputName: 'centerLatitude', inputValues: ['-90.5', '90.5', null, 'abc'], isValid: false},
        {inputName: 'borderLatitude', inputValues: ['-90.5', '90.5', null, 'abc'], isValid: false},
        {inputName: 'centerLongitude', inputValues: ['-180', '180', '0.52'], isValid: true},
        {inputName: 'borderLongitude', inputValues: ['-180', '180', '0.52'], isValid: true},
        {inputName: 'centerLatitude', inputValues: ['-90', '90', '0.52'], isValid: true},
        {inputName: 'borderLatitude', inputValues: ['-90', '90', '0.52'], isValid: true},
    ]
    
    geoCoordinatesTestCases.forEach(({inputName, inputValues, isValid}) => {
        it(`should validate geo coordinates parameters`, () => {
            const form = component.geoDataForm;
            const input = form.controls[inputName];
            inputValues.forEach(value => {
                input.setValue(value);
                expect(input.valid).toBe(isValid);
            })
        })
    })

    it('should test whole form validity', () => {
        const form = component.geoDataForm;
        form.controls.centerLongitude.setValue('-119');
        form.controls.centerLatitude.setValue('35');
        form.controls.borderLongitude.setValue('-122');
        form.controls.borderLatitude.setValue('40');
        expect(form.valid).toBeTruthy();
        form.controls.borderLatitude.setValue('940');
        expect(form.valid).toBeFalsy();
    });

    it('should update countResult from service', () => {
        expect(component.countResult).toBe(2);
    })



});