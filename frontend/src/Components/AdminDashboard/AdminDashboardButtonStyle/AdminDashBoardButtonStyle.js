import React,{useState} from 'react';
import { useHistory } from 'react-router-dom';
import Modal from 'react-modal';
import NotImplemented from '../../NotImplemented/NotImplemented';
import './AdminDashboardButtonStyle.css';


function AdminDashBoardButtonStyle(props) {
  const history=useHistory();
  const [modalIsOpen, setModalIsOpen] = useState(false);

  const setModalIsOpenToTrue =()=>{
      setModalIsOpen(true)
  }
  const setModalIsOpenToFalse =()=>{
    setModalIsOpen(false)
}
  function handleClick(props){
    if(props==='/dashboard')
    {
      history.push('/dashboard');
    }
    else{
      setModalIsOpenToTrue();
    }
  }

  return (
    <div className='AdminDashboardButton'>
      <div className='button-body'>
        <div style={{ paddingTop: '.8em' }}>
          <img className='icon' src={props.item.link_icon} alt='logo' />
        </div>
        <div style={{ paddingTop: '1em' }}>
          <button onClick={()=>handleClick(props.item.component_class)} className='service-title'>{props.item.link_title}</button>
          <Modal   style={{
    overlay: {
      position: 'fixed',
      top: 0,
      left: 0,
      right: 0,
      bottom: 0,
      backgroundColor: 'rgba(255, 255, 255, 0.75)'
    },
    content: {
      inset:'400px ',
      width:'800px',
      position: 'absolute',
      marginLeft:'8%',
      border: '1px solid #ccc',
      background: '#fff',
      WebkitOverflowScrolling: 'touch',
      borderRadius: '4px',
      outline: 'none',
      padding: '20px',
      backgroundColor:'white'
    }
  }} isOpen={modalIsOpen}>
                <button onClick={setModalIsOpenToFalse}>x</button>
                <NotImplemented/>
            </Modal>
        </div>
        <div style={{ padding: '.9em', fontSize: '1.3em', color: 'grey' }}>
          <a href={props.item.component_class} className='service-title-arrow'>
            &#62;
          </a>
        </div>
      </div>
    </div>
  );
}

export default AdminDashBoardButtonStyle;
