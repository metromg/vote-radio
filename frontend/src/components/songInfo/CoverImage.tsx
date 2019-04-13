import React from 'react';

interface CoverImageProps {
    url: string | null;
}

const CoverImage = (props: CoverImageProps) => {
    const hasCoverImage = props.url != null;
    const style = { backgroundImage: hasCoverImage ? `url(${props.url})` : undefined };

    return (
        <div className="cover" style={style}>
            {hasCoverImage ? null : <span>×</span>}
        </div>
    );
}

export default CoverImage;