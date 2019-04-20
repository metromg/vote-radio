import React from 'react';

interface ProgressBarProps {
    totalDurationInSeconds: number;
    remainingDurationInSeconds: number;
}

export const ProgressBar = (props: ProgressBarProps) => {
    const percentage = (props.remainingDurationInSeconds / props.totalDurationInSeconds) * 100;

    return (
        <div className="playback-progress">
            <div className="time-indicator" style={{ left: percentage + "%" }}>
                {toTimeString(props.remainingDurationInSeconds)}
            </div>
            <div className="bar" style={{ width: percentage + "%" }}></div>
        </div>
    );
};

function toTimeString(durationInSeconds: number) {
    var minutes = Math.floor(durationInSeconds / 60);
    var seconds = Math.floor(durationInSeconds - minutes * 60);

    return padLeft(minutes.toString(), '0', 2) + ':' + padLeft(seconds.toString(), '0', 2);
}

function padLeft(string: string, pad: string, length: number) {
    return (new Array(length+1).join(pad)+string).slice(-length);
}

export default ProgressBar;