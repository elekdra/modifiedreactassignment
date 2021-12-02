import React, { useRef, useState, useEffect } from 'react';
import './Form.css';
import filter from '../../../assets/filter.png';
import upload from '../../../assets/upload.png';
import { useHistory } from 'react-router-dom';
import { CompanyNames } from '../../../ApiServices/ApiServices';
import { TrainingNames } from '../../../ApiServices/ApiServices';
import { FilterData } from '../../../ApiServices/ApiServices';


function Form(props) {
  const history = useHistory();
  const dataForm = useRef(null);
  const [company, setCompany] = useState('ALL');
  const [version, setVersion] = useState();
  const [training, setTraining] = useState('ALL');
  const [companies, setCompanies] = useState([]);
  const [trainings, setTrainings] = useState([]);

  // company name data api calll
  const fetchCompanyData = () => {
    CompanyNames().then((value) => {
      console.log(value.data);
      let companies = value.data.split('|');
      console.log(companies);
      setCompanies(companies);
    });
  };
  useEffect(() => {
    fetchCompanyData();
  }, []);


  // training name data api call
  const fetchTrainingData = () => {
    TrainingNames().then((value) => {
      console.log(value.data);
      let trainings = value.data.split('|');

      setTrainings(trainings);
    });
  };
  useEffect(() => {
    fetchTrainingData();
  }, []);

  //upload button onclick invokes here
  function handleChange() {
    console.log(props.item);
    props.item.setTraining(training);
    props.item.setCompany(company);
    props.item.setVersion(version);
    history.push('/uploaddocument', {
      fullFileItem: props.fileItems,
      companies: companies,
      trainings: trainings,
    });
  }

  // Filter data api calls here
  function handleClick(e) {
    e.preventDefault();
    FilterData(company, version, training).then((response) => {
      let fullData = response.data;
      fullData.forEach((item) => {
        let result = item.fileContent.indexOf('files');
        let tempName = item.fileContent.slice(
          result + 5,
          item.fileContent.length
        );
        item.fileContent = 'http://localhost:5000/files/' + tempName;
      });
      props.setFullFileData(fullData);
    });
  }
  return (
    <form ref={dataForm}>
      <div className='data-form'>
        <div>
          <div>Company</div>
        </div>
        <div>
          <div>Version</div>
        </div>
        <div>
          <div>Training</div>
        </div>
        <div>
          <div></div>
        </div>
        <div>
          <div></div>
        </div>
        <div>
          <div>
            <select
              onChange={(e) => {
                setCompany(e.target.value);
              }}
              id='company'
            >
              <option value='ALL' selected={company == 'ALL'}>
                ALL
              </option>
              {companies.map((item) => (
                <option value={item} selected={company == { item }}>
                  {item}
                </option>
              ))}
            </select>
          </div>
        </div>
        <div>
          <div>
            <input
              type='text'
              id='version'
              onChange={(e) => {
                setVersion(e.target.value);
              }}
            />
          </div>
        </div>
        <div>
          <div>
            <select
              id='training'
              onChange={(e) => {
                setTraining(e.target.value);
              }}
            >
              <option value='ALL' selected={training == 'ALL'}>
                ALL
              </option>
              {trainings.map((item) => (
                <option value={item} selected={training == { item }}>
                  {item}
                </option>
              ))}
            </select>
          </div>
        </div>
        <div>
          <div>
            <button className='filter-button' onClick={handleClick}>
              <img src={filter} alt='' />
              <span>Filter</span>
            </button>
          </div>
        </div>
        <div>
          <div>
            <button
              className='upload-button'
              onClick={() => {
                handleChange();
              }}
            >
              {' '}
              <img src={upload} alt='' />
              <span>Upload</span>
            </button>
          </div>
        </div>
      </div>
    </form>
  );
}

export default Form;
