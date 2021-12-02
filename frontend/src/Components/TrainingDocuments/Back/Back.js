import React from 'react';
import { useHistory } from 'react-router-dom';
import arrow from '../../../assets/arrow.png';
import './Back.css';

function Back(props) {
  let history = useHistory();
  function BacktoAdminPage() {
    if (props.page === '/trainingdocuments') {
      history.push('/trainingdocuments',props.username);
    } else {
      history.push('/dashboard');
    }
  }
  return (
    <div>
      <button className='back-button' onClick={BacktoAdminPage}>
        <img src={arrow} alt='' />
        <span>Back</span>
      </button>
    </div>
  );
}

export default Back;
