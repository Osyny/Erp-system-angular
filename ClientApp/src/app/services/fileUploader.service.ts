import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Subject } from "rxjs";
import { IFileData } from "./files.service";


@Injectable({
    providedIn: 'root'
})
export class FileUploaderService {
    newFileDataReaded: Subject<IFileData> = new Subject();
    fileUploaded: Subject<void> = new Subject();
    private fileDataArray: IFileData[] = [];

    private file!: File;
    private fileData: string = '';


    constructor(
        // protected toastr: ToastrService,
        // private modalService: BsModalService,
        // private platform: Platform,
        private http: HttpClient
    ) {
     }

    get File(): File {
        return this.file;
    }

    get FileData(): string {
        return this.fileData;
    }

    get FileDataArray(): IFileData[] {
        return this.fileDataArray;
    }

    base64ToBlob(input: { data: string; type: any; name: string; }): Blob {
        try {
            let data = input.data.split(',');
            const byteString = atob(data[1]);
            const arrayBuffer = new ArrayBuffer(byteString.length);
            const int8Array = new Uint8Array(arrayBuffer);
            for (let i = 0; i < byteString.length; i++) {
                int8Array[i] = byteString.charCodeAt(i);
            }
            return new Blob([int8Array], { type: input.type });
        } catch(e) {
            return new Blob([], { type: input.type });
        }
    }

    attachSelectedPhotos(formData: FormData): FormData {

        debugger
        for (const item of this.FileDataArray) {
            let blob = this.base64ToBlob({data: item.fileData, type: 'image/png', name: item.name });
            formData.append('files[]', blob, item.name);
        }
        return formData;
    }
}