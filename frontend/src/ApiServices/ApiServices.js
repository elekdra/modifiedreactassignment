import axios from 'axios';

const CompanyNames=()=> {
  console.log('enter here');
  console.log('Company names');
  let ApiUrl = 'http://localhost:5000/api/TrainingDocumentsScreen/company/';
  let DefaultValue = axios.get(ApiUrl);
  console.log(DefaultValue);
  return DefaultValue;
}

const TrainingNames=()=> {
  let ApiUrl = 'http://localhost:5000/api/TrainingDocumentsScreen/training/';
  let DefaultValue = axios.get(ApiUrl);
  console.log(DefaultValue);
  return DefaultValue;
}

const FilterData=(company, version, training)=> {
    console.log('filter the data');
    let ApiUrl =
      'http://localhost:5000/api/TrainingDocumentsScreen/getFilteredData/?filterParameters=' +
      company +
      '|' +
      version +
      '|' +
      training;
    let DefaultValue = axios.get(ApiUrl);
    console.log(DefaultValue);
    return DefaultValue;
  }

const StartUpDefaultsValue=()=> {
    let ApiUrl =
      'http://localhost:5000/api/FileUploadScreen/getdefaults/?initialize=StartupInitiation';
    let DefaultValue = axios.get(ApiUrl);
    console.log(DefaultValue);
    return DefaultValue;
  }

  const UserAuthentication=(username, password)=> {
    let ApiUrl =
      'http://localhost:5000/api/LoginScreen/authenticate?credentials=' +
      username +
      '|' +
      password;
    let response = axios.get(ApiUrl);
    console.log(response.data);
    return response;
  }

 const FileCheck=(company, version, training, filename)=> {
    let ApiUrl =
      'http://localhost:5000/api/FileUploadScreen/getFileCheck/?FileParameters=' +
      company +
      '|' +
      version +
      '|' +
      training +
      '|' +
      filename;
    let DefaultValue = axios.get(ApiUrl);
    return DefaultValue;
  }
  
  
export {CompanyNames,TrainingNames,FilterData,StartUpDefaultsValue,UserAuthentication,FileCheck};