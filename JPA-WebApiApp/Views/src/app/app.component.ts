import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {
  title = 'Base64 Converter';
  myForm: FormGroup;
  private abortController: AbortController | undefined;
  ngtxtOutput: string = '';
  isButtonConvertDisabled: boolean = false;
  isButtonCancelDisabled: boolean = true;
  loadingDots = '';

  constructor() {
    this.myForm = new FormGroup({
      textInput: new FormControl('', [Validators.required]),
    });
  }

  ngOnInit() {
    // Create dynamic ellipsis dots
    setInterval(() => {
      this.loadingDots = (this.loadingDots === '...') ? '' : (this.loadingDots + '.');
    }, 1000);
  }

  convertText() {
    var isInvalid = this.myForm.get('textInput')?.invalid;

    this.myForm.get('textInput')?.markAsTouched();
    this.ngtxtOutput = "";

    if (!isInvalid) {
      console.log('INPUT TEXT: ' + this.myForm.get('textInput')?.value);
      this.isButtonConvertDisabled = true;
      this.isButtonCancelDisabled = false;
      this.fetchDataFromApi();
    }
  }

  cancelCurrentRequest() {
    if (this.abortController) {
      this.abortController.abort();
      this.abortController = undefined;
      this.isButtonConvertDisabled = false;
      this.isButtonCancelDisabled = true;
    }
  }

  async fetchDataFromApi() {
    try {

      this.abortController = new AbortController();
      const signal = this.abortController.signal;

      //api call using fetch
      const response = await fetch('http://localhost:5292/api/Converter?text=' + this.myForm.get('textInput')?.value, {
        method: 'GET',
        signal,
      });

      //result validation
      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }

      if (!response.body) {
        throw new Error("Response body is not available or not supported.");
      }

      //checking response
      const reader = response.body?.getReader();

      //iterate until its done
      while (true) {
        const { done, value } = await reader.read();

        if (done) {
          console.log("ENCODED BASE64 : " + this.ngtxtOutput);
          this.isButtonConvertDisabled = false;
          this.isButtonCancelDisabled = true;
          break;
        }

        //text decoder for result
        const textDecoder = new TextDecoder('utf-8');
        const decodedString = this.removeChars(textDecoder.decode(value));
        this.ngtxtOutput += decodedString;
      }
    }
    catch (error: any)
    {
      if (error instanceof Error) {
        this.isButtonConvertDisabled = false;
        this.isButtonCancelDisabled = true;
        console.log(error.message);
        this.ngtxtOutput = error.message;
      }
    }
  }

  removeChars(inputString: string): string {
    inputString = inputString.replaceAll("\"", "");
    inputString = inputString.replaceAll("[", "");
    inputString = inputString.replaceAll("]", "");
    inputString = inputString.replaceAll(",", "");
    return inputString;
  }
}
