import React from 'react';
import './AdminHeader.css';

//admin header
function AdminHeader(props) {
  return (
    <div>
      <h1>{props.title}</h1>
    </div>
  );
}

export default AdminHeader;
