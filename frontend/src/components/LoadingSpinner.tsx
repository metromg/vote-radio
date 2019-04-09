import React from 'react';

import './LoadingSpinner.css';

interface LoadingSpinnerProps {
}

const LoadingSpinner = (props: LoadingSpinnerProps) => {
    return (
        <div className="lds-spinner">
            <div></div>
            <div></div>
            <div></div>
            <div></div>
            <div></div>
            <div></div>
            <div></div>
            <div></div>
            <div></div>
            <div></div>
            <div></div>
            <div></div>
        </div>
    );
}

export default LoadingSpinner;