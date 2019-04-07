import React, { ReactNode } from 'react';

interface CoverImageProps {
    url: string | null;
    children: ReactNode;
}

const CoverImage = (props: CoverImageProps) => {
    const style = props.url != null
        ? { backgroundImage: `url(${props.url})` }
        : undefined;

    return (
        <div className="playback-mobile-cover" style={style}>
            {props.children}
            <div className="playback-desktop-cover" style={style}></div>
        </div>
    );
}

export default CoverImage;