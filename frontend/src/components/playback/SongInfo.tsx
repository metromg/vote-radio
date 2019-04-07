import React from 'react';
import { Translate } from 'react-localize-redux';

interface SongInfoProps {
    title: string;
    album?: string | null;
    artist?: string | null;

    voteCount: number;
}

const SongInfo = (props: SongInfoProps) => {
    return (
        <React.Fragment>
            <div className="playback-song-description">
                <div className="playback-subtitle">
                    {props.artist}
                    {props.artist && props.album ? " - " : null}
                    {props.album}
                </div>
                <div className="playback-title">
                    {props.title}
                </div>
            </div>
            <div className="playback-voting-info">
                {props.voteCount}&nbsp;
                {
                    props.voteCount === 1
                        ? <Translate id="vote" />
                        : <Translate id="votes" />
                }
            </div>
        </React.Fragment>
    );
}

export default SongInfo;